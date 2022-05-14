package com.example.scanner.data.repository

import com.example.scanner.data.storage.GarbageStorage
import com.example.scanner.data.storage.models.GarbageCategory
import com.example.scanner.domain.models.Barcode
import com.example.scanner.domain.models.GarbageInfo

import com.example.scanner.domain.repository.GarbageRepository

class GarbageRepositoryImpl(val garbageStorage: GarbageStorage) : GarbageRepository {
    override fun getGarbageInfoByBarcode(barcode: Barcode): GarbageInfo {
        val dataBarcode = mapDomainBarcodeToDataBarcode(barcode)
        val dataResult = garbageStorage.getGarbageInfoByBarcode(dataBarcode)
        return mapDataGarbageInfoToDomainGarbageInfo(dataResult)
    }

    private fun mapDomainBarcodeToDataBarcode(barcode: Barcode): com.example.scanner.data.storage.models.Barcode {
        return com.example.scanner.data.storage.models.Barcode(barcode.barcode)
    }

    private fun mapDataGarbageInfoToDomainGarbageInfo(garbageInfo: com.example.scanner.data.storage.models.GarbageInfo): com.example.scanner.domain.models.GarbageInfo {
        val domainGarbageCategories = List<com.example.scanner.domain.models.GarbageCategory>(
            garbageInfo.garbageCategories.size,
            { com.example.scanner.domain.models.GarbageCategory(garbageInfo.garbageCategories[it].name) })
        return GarbageInfo(garbageInfo.name, garbageInfo.description, domainGarbageCategories)
    }

    private fun mapDataGarbageCategoryToDomainGarbageCategory(garbageCategory: GarbageCategory): com.example.scanner.domain.models.GarbageCategory {
        return com.example.scanner.domain.models.GarbageCategory(garbageCategory.name)
    }
}