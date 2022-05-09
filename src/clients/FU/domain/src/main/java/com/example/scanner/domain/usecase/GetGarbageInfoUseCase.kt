package com.example.scanner.domain.usecase

import com.example.scanner.domain.models.Barcode
import com.example.scanner.domain.models.GarbageInfo
import com.example.scanner.domain.repository.GarbageRepository

class GetGarbageInfoUseCase(val garbageRepository: GarbageRepository) {

    fun execute(barcode: Barcode): GarbageInfo{
        return garbageRepository.getGarbageInfoByBarcode(barcode)
    }
}