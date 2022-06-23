package com.example.scanner.data.storage.retrofit2

import com.example.scanner.data.storage.GarbageStorage
import com.example.scanner.data.storage.models.*
import com.example.scanner.data.storage.retrofit2.interfaces.GarbageService
import okhttp3.MediaType
import okhttp3.MultipartBody
import okhttp3.RequestBody
import retrofit2.*
import java.net.ConnectException
import java.net.SocketTimeoutException


class GarbageStorageImpl: GarbageStorage {
    override  fun getGarbageInfoByBarcode(barcode: Barcode): ApiResultSingle<GetGarbageInfo> {
        val garbageService: GarbageService = RetrofitClient.getClient().create(GarbageService::class.java)


        var result: ApiResultSingle<GetGarbageInfo>
        try {
            val call = garbageService.getGarbageInfoByBarcode(barcode.barcode)
            val response = call.execute();
            if (response.isSuccessful) {
                result = response.body()!!
                result.responseCode = response.code()
                return result
            }
            result = ApiResultSingle(false, listOf("При обработке запроса произошла ошибка!"), null, responseCode = response.code())
            return result
        }
        catch (e: ConnectException){
            return ApiResultSingle(false, listOf("Не удалось отправить запрос!"), null, -1)
        }
    }

    override  fun addGarbageInfo(garbageInfo: GarbageInfo): ApiResult<String> {
        val garbageService: GarbageService = RetrofitClient.getClient().create(GarbageService::class.java)


        var result: ApiResult<String>
        try {
            val garbageCategories = List(garbageInfo.garbageCategories.size){
                garbageInfo.garbageCategories[it].id
            }

            val barcodeBody: RequestBody =
                RequestBody.create(MediaType.parse("text/plain"), garbageInfo.barcode)
            val nameBody: RequestBody =
                RequestBody.create(MediaType.parse("text/plain"), garbageInfo.name)
            val descriptionBody: RequestBody =
                RequestBody.create(MediaType.parse("text/plain"), garbageInfo.description)
            var imageBody: MultipartBody.Part? = null
            if(garbageInfo.image != null){
                val reqFile: RequestBody = RequestBody.create(MediaType.parse("image/*"), garbageInfo.image)
                imageBody =
                    MultipartBody.Part.createFormData("Image", garbageInfo.image.getName(), reqFile)
            }
            val call = garbageService.addGarbageInfo(nameBody, descriptionBody,barcodeBody, imageBody, garbageCategories)
            val response = call.execute();

            if (response.isSuccessful) {
                result = response.body()!!
                result.responseCode = response.code()
                return result
            }
            result = ApiResult(false, listOf("При обработке запроса произошла ошибка!"), emptyList(), responseCode = response.code())
            return result
        }
        catch (e: ConnectException){
            return ApiResult(false, listOf("Не удалось отправить запрос!"), emptyList(), -1)
        }
        catch (e: SocketTimeoutException){
            return ApiResult(false, listOf("Превышено время ожидания ответа!"), emptyList(), -1)
        }
    }

    override fun getGarbageCategories(): ApiResult<GarbageCategory> {
        val garbageService: GarbageService = RetrofitClient.getClient().create(GarbageService::class.java)

        var result: ApiResult<GarbageCategory>
        try {
            val call = garbageService.getGarbageCategories()
            val response = call.execute();

            if (response.isSuccessful) {
                result = response.body()!!
                result.responseCode = response.code()
                return result
            }
            result = ApiResult(false, listOf("При обработке запроса произошла ошибка!"), emptyList(), responseCode = response.code())
            return result
        }
        catch (e: ConnectException){
            return ApiResult(false, listOf("Не удалось отправить запрос!"), emptyList(), -1)
        }
    }
}