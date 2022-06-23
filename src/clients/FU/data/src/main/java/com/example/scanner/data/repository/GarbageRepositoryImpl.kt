package com.example.scanner.data.repository

import com.example.scanner.data.storage.GarbageStorage
import com.example.scanner.data.storage.models.ApiResult
import com.example.scanner.data.storage.models.ApiResultSingle
import com.example.scanner.data.storage.models.GarbageCategory
import com.example.scanner.data.storage.models.GetGarbageInfo
import com.example.scanner.domain.models.Barcode
import com.example.scanner.domain.models.GarbageInfo
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.domain.models.RequestResultSingle

import com.example.scanner.domain.repository.GarbageRepository
import com.example.scanner.data.storage.models.GarbageInfo as dataGarbageInfo
import com.example.scanner.domain.models.GarbageCategory as dataGarbageCategory
import com.example.scanner.domain.models.GarbageCategory as domainCategory
import com.example.scanner.domain.models.GetGarbageInfo as domainGetGarbageInfo

class GarbageRepositoryImpl(val garbageStorage: GarbageStorage) : GarbageRepository {
    override  fun getGarbageInfoByBarcode(barcode: Barcode): RequestResultSingle<domainGetGarbageInfo> {
        val dataBarcode = mapBarcode(barcode)


        val dataResult = garbageStorage.getGarbageInfoByBarcode(dataBarcode)
        return mapRequestResult(dataResult)
    }

    override fun addGarbageInfo(garbageInfo: GarbageInfo): RequestResult<String> {
        val dataGarbageInfo = mapGarbageInfo(garbageInfo)
        val apiResult = garbageStorage.addGarbageInfo(dataGarbageInfo)

        val data: List<String>? = null
        var dataCount = 0

        var messages = listOf<String>()
        if(apiResult.messages != null){
            messages = apiResult.messages
        }

        return RequestResult(apiResult.success, messages, data, dataCount, apiResult.responseCode)
    }

    override fun getGarbageCategories(): RequestResult<domainCategory> {
        val apiResult = garbageStorage.getGarbageCategories()
        var data = emptyList<domainCategory>()
        var messages = emptyList<String>()
        if(apiResult.data != null){
            data = mapGarbageCategories(apiResult.data!!)
        }
        if(apiResult.messages != null){
            messages = apiResult.messages
        }
        return RequestResult(apiResult.success, messages, data, data.size, apiResult.responseCode)
    }

    private fun mapBarcode(barcode: Barcode): com.example.scanner.data.storage.models.Barcode {
        return com.example.scanner.data.storage.models.Barcode(barcode.barcode)
    }

    private fun mapRequestResult(apiResult: ApiResultSingle<GetGarbageInfo>): RequestResultSingle<domainGetGarbageInfo> {
        var data: domainGetGarbageInfo? = null
        if(apiResult.data != null) {
            val categories = List<domainCategory>(apiResult.data!!.garbageCategories!!.size){
                domainCategory(apiResult.data!!.garbageCategories!![it].name, apiResult.data!!.garbageCategories!![it].id)
            }
            data = domainGetGarbageInfo(apiResult.data!!.name, apiResult.data!!.description, categories, apiResult.data!!.barcode, apiResult.data!!.imagePath)
        }

        var messages = emptyList<String>()

        if(apiResult.messages != null){
            messages = apiResult.messages
        }

        return RequestResultSingle(apiResult.success, messages, data, apiResult.responseCode)
    }

    private fun mapGarbageCategories(garbageCategories: List<GarbageCategory>): List<dataGarbageCategory>{
        val domainGarbageCategories = List<dataGarbageCategory>(
            garbageCategories.size
        ) { dataGarbageCategory(garbageCategories[it].name, garbageCategories[it].id) }
        return domainGarbageCategories
    }

    private fun mapGarbageInfo(dataGarbageInfo: dataGarbageInfo) : GarbageInfo{
        return GarbageInfo(dataGarbageInfo.name, dataGarbageInfo.description, mapGarbageCategories(dataGarbageInfo.garbageCategories),
            dataGarbageInfo.barcode, dataGarbageInfo.image)
    }

    private fun mapGarbageInfo(garbageInfo: GarbageInfo): dataGarbageInfo{
        val garbageCategories = List(garbageInfo.garbageCategories!!.size){
            GarbageCategory("",garbageInfo.garbageCategories!![it].id)
        }
        return dataGarbageInfo(garbageInfo.name, garbageInfo.description, garbageCategories, garbageInfo.barcode, garbageInfo.image)
    }
}