package com.example.scanner.data.storage.retrofit2

import com.example.scanner.data.storage.GarbageStorage
import com.example.scanner.data.storage.models.Barcode
import com.example.scanner.data.storage.models.GarbageInfo
import com.example.scanner.data.storage.retrofit2.interfaces.GarbageService
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory


class GarbageStorageImpl: GarbageStorage {
    override fun getGarbageInfoByBarcode(barcode: Barcode): GarbageInfo {
        val garbageService: GarbageService = RetrofitClient.getClient().create(GarbageService::class.java)

        var result: GarbageInfo? = null
        garbageService.getGarbageInfoByBarcode(barcode.barcode).enqueue(object:
            Callback<GarbageInfo> {
            override fun onResponse(call: Call<GarbageInfo>, response: Response<GarbageInfo>) {
                result = response.body()
                result?.success = true
            }

            override fun onFailure(call: Call<GarbageInfo>, t: Throwable) {
                result?.success = false;
            }
        })
        return result!!
    }
}