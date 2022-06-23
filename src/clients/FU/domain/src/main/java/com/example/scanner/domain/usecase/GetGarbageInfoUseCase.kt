package com.example.scanner.domain.usecase

import com.example.scanner.domain.models.*
import com.example.scanner.domain.repository.GarbageRepository

class GetGarbageInfoUseCase(val garbageRepository: GarbageRepository) {

    fun execute(barcode: Barcode): RequestResultSingle<GetGarbageInfo>{
        return garbageRepository.getGarbageInfoByBarcode(barcode)
    }
}