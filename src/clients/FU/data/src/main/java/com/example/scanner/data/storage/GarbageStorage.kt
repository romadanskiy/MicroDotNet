package com.example.scanner.data.storage

import com.example.scanner.data.storage.models.Barcode
import com.example.scanner.data.storage.models.GarbageInfo
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Query

interface GarbageStorage {

    fun getGarbageInfoByBarcode(barcode: Barcode): GarbageInfo
}