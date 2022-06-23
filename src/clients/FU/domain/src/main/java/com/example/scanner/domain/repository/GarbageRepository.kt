package com.example.scanner.domain.repository

import com.example.scanner.domain.models.*

interface GarbageRepository {

    fun getGarbageInfoByBarcode(barcode: Barcode) : RequestResultSingle<GetGarbageInfo>

    fun addGarbageInfo(garbageInfo: GarbageInfo) : RequestResult<String>

    fun getGarbageCategories(): RequestResult<GarbageCategory>
}