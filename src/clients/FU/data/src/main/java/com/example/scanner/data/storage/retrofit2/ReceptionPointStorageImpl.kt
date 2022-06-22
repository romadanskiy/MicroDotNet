package com.example.scanner.data.storage.retrofit2

import com.example.scanner.data.storage.ReceptionPointStorage
import com.example.scanner.data.storage.models.ApiResult
import com.example.scanner.data.storage.models.ApiResultSingle
import com.example.scanner.data.storage.models.GetReceptionPoint
import com.example.scanner.data.storage.models.ReceptionPoint
import com.example.scanner.data.storage.retrofit2.interfaces.ReceptionPointService
import okhttp3.MediaType
import okhttp3.RequestBody
import java.net.ConnectException

class ReceptionPointStorageImpl: ReceptionPointStorage {
    override fun getReceptionPoints(categoryIds: List<Long>?): ApiResult<GetReceptionPoint> {
        val receptionPointService: ReceptionPointService = RetrofitClient.getClient().create(
            ReceptionPointService::class.java)


        var result: ApiResult<GetReceptionPoint>
        try {
            val call = receptionPointService.getReceptionPoints(categoryIds)
            val response = call.execute();
            if (response.isSuccessful) {
                result = response.body()!!
                result.responseCode = response.code()
                return result
            }
            result = ApiResult(false, listOf("При обработке запроса произошла ошибка!"), null, responseCode = response.code())
            return result
        }
        catch (e: ConnectException){
            return ApiResult(false, listOf("Не удалось отправить запрос!"), null, -1)
        }
    }

    override fun addReceptionPoint(receptionPoint: ReceptionPoint): ApiResult<String> {
        val receptionPointService: ReceptionPointService = RetrofitClient.getClient().create(
            ReceptionPointService::class.java)


        var result: ApiResult<String>
        try {
            val nameBody: RequestBody =
                RequestBody.create(MediaType.parse("text/plain"), receptionPoint.name)
            val descriptionBody: RequestBody =
                RequestBody.create(MediaType.parse("text/plain"), receptionPoint.description)
            val addressBody: RequestBody =
                RequestBody.create(MediaType.parse("text/plain"), receptionPoint.address)

            val call = receptionPointService.addReceptionPoint(nameBody, descriptionBody, addressBody, receptionPoint.garbageTypes)
            val response = call.execute();
            if (response.isSuccessful) {
                result = response.body()!!
                result.responseCode = response.code()
                return result
            }
            result = ApiResult(false, listOf("При обработке запроса произошла ошибка!"), null, responseCode = response.code())
            return result
        }
        catch (e: ConnectException){
            return ApiResult(false, listOf("Не удалось отправить запрос!"), null, -1)
        }
    }
}