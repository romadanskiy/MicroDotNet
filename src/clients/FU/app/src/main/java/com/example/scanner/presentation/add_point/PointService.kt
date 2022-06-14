package com.example.scanner.presentation.add_point

import com.example.scanner.data.storage.models.BaseResponse
import io.reactivex.Single
import retrofit2.http.Body
import retrofit2.http.POST

interface PointService {

    @POST("ReceptionPoint/AddReceptionPoint")
    fun addPoint(
        @Body body: PointBody
    ): Single<BaseResponse<PointResponse>>

}

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
