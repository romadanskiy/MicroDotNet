package com.example.scanner.interfaces

import com.example.scanner.models.dto.GarbageInfo
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Query


interface GarbageApi {
    @GET("garbage")
    fun getGarbageInfo(@Query("barcode") barcode: String): Call<GarbageInfo>?
}