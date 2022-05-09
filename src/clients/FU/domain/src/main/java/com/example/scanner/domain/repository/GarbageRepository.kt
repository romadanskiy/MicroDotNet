package com.example.scanner.domain.repository

import com.example.scanner.domain.models.Barcode
import com.example.scanner.domain.models.GarbageInfo

interface GarbageRepository {

    fun getGarbageInfoByBarcode(barcode: Barcode) : GarbageInfo
}