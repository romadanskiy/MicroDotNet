package com.example.scanner.data.storage

import com.example.scanner.data.storage.models.*
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Query

interface GarbageStorage {

     fun getGarbageInfoByBarcode(barcode: Barcode): ApiResultSingle<GetGarbageInfo>

     fun addGarbageInfo(garbageInfo: GarbageInfo): ApiResult<String>

     fun getGarbageCategories(): ApiResult<GarbageCategory>
}