package com.example.scanner.data.storage.retrofit2.interfaces

import android.graphics.Bitmap
import com.example.scanner.data.storage.models.*
import okhttp3.MultipartBody
import okhttp3.RequestBody
import retrofit2.Call
import retrofit2.http.*

const val CACHE_CONTROL_HEADER = "Cache-Control"
const val CACHE_CONTROL_NO_CACHE = "no-cache"

interface GarbageService {
    @GET("garbage")
    @Headers("$CACHE_CONTROL_HEADER: $CACHE_CONTROL_NO_CACHE")
    fun getGarbageInfoByBarcode(@Query("barcode") barcode: String): Call<ApiResultSingle<GetGarbageInfo>>

    @Multipart
    @POST("garbage/AddGarbageInfo")
    fun addGarbageInfo(@Part("name") name: RequestBody, @Part("description") description: RequestBody?, @Part("barcode") barcode: RequestBody,
                       @Part image: MultipartBody.Part?, @Part("GarbageTypes") garbageTypes: List<Long>): Call<ApiResult<String>>

    @GET("garbage/GetGarbageTypes")
    @Headers("$CACHE_CONTROL_HEADER: $CACHE_CONTROL_NO_CACHE")
    fun getGarbageCategories() : Call<ApiResult<GarbageCategory>>
}
