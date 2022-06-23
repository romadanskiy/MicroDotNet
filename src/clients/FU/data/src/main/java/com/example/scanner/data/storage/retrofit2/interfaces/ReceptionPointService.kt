package com.example.scanner.data.storage.retrofit2.interfaces

import com.example.scanner.data.storage.models.*
import okhttp3.MultipartBody
import okhttp3.RequestBody
import retrofit2.Call
import retrofit2.http.*

interface ReceptionPointService {

    @GET("receptionPoint/GetReceptionPoints")
    @Headers("$CACHE_CONTROL_HEADER: $CACHE_CONTROL_NO_CACHE")
    fun getReceptionPoints(
        @Query("garbageTypeIds") garbageTypes: List<Long>?
    ): Call<ApiResult<GetReceptionPoint>>

    @Multipart
    @POST("receptionPoint/AddReceptionPoint")
    fun addReceptionPoint(
        @Part("name") name: RequestBody,
        @Part("description") description: RequestBody,
        @Part("address") address: RequestBody,
        @Part("garbageTypeIds") garbageTypes: List<Long>,
        @Part("longitude") longitude: String = "longitude",
        @Part("latitude") latitude: String = "latitude"
    ): Call<ApiResult<String>>
}