package com.example.scanner.data.storage.retrofit2.interfaces

import com.example.scanner.data.storage.models.Barcode
import com.example.scanner.data.storage.models.GarbageInfo
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Query


interface GarbageService {
    @GET("garbage")
    fun getGarbageInfoByBarcode(@Query("barcode") barcode: String): Call<GarbageInfo>
}
