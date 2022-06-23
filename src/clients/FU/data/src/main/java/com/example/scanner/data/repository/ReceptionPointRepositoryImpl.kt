package com.example.scanner.data.repository

import com.example.scanner.data.storage.ReceptionPointStorage
import com.example.scanner.data.storage.models.ApiResultSingle
import com.example.scanner.data.storage.models.GetGarbageInfo
import com.example.scanner.data.storage.retrofit2.RetrofitClient
import com.example.scanner.data.storage.retrofit2.interfaces.GarbageService
import com.example.scanner.data.storage.retrofit2.interfaces.ReceptionPointService
import com.example.scanner.domain.models.Address
import com.example.scanner.domain.models.GetReceptionPoint
import com.example.scanner.domain.models.ReceptionPoint
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.domain.repository.ReceptionPointRepository
import okhttp3.MediaType
import okhttp3.RequestBody
import java.net.ConnectException
import com.example.scanner.data.storage.models.GetReceptionPoint as dataGetReceptionPoint

class ReceptionPointRepositoryImpl(val receptionPointStorage: ReceptionPointStorage): ReceptionPointRepository {
    override fun getReceptionPoints(categoryIds: List<Long>?): RequestResult<GetReceptionPoint> {
        val apiResult = receptionPointStorage.getReceptionPoints(categoryIds)

        var messages = emptyList<String>()
        if( apiResult.messages!=null){
            messages = apiResult.messages
        }
        var data = emptyList<GetReceptionPoint>()
        if (apiResult.data != null){
            data = List(apiResult.data.size){
                GetReceptionPoint(apiResult.data[it].id,apiResult.data[it].name, apiResult.data[it].description, Address(apiResult.data[it].address.fullAddress), apiResult.data[it].garbageTypes)
            }
        }
        val requestResult = RequestResult<GetReceptionPoint>(apiResult.success, messages, data, data.size, apiResult.responseCode)
        return requestResult
    }

    override fun addReceptionPoint(receptionPointInfo: ReceptionPoint): RequestResult<String> {
        val dataReceptionPoint = com.example.scanner.data.storage.models.ReceptionPoint(receptionPointInfo.name, receptionPointInfo.description, receptionPointInfo.address, receptionPointInfo.garbageTypes)
        val apiResult = receptionPointStorage.addReceptionPoint(dataReceptionPoint)

        var messages = emptyList<String>()
        if( apiResult.messages!=null){
            messages = apiResult.messages
        }
        var data = emptyList<String>()
        if (apiResult.data != null){
            data = apiResult.data
        }
        val requestResult = RequestResult<String>(apiResult.success, messages, data, data.size, apiResult.responseCode)
        return requestResult
    }
}