package com.example.scanner.presentation.add_point

import com.example.scanner.data.storage.models.BaseResponse
import io.reactivex.Single
import retrofit2.http.Body
import retrofit2.http.Header
import retrofit2.http.POST

interface PointService {

    @POST("ReceptionPoint/AddReceptionPoint")
    fun addPoint(
        @Header(HEADER_TOKEN) token: String,
        @Body body: PointBody
    ): Single<BaseResponse<PointResponse>>

    @POST("ReceptionPoint/GetReceptionPoints")
    fun getPoints()

}

const val HEADER_TOKEN = "X-Bearer-Token"

data class PointResponse(
    val data: Nothing
)

data class PointBody(
    val name: String,
    val description: String,
    val address: String,
    val longitude: String,
    val latitude: String,
    val garbageTypeIds: Int = 1
)
