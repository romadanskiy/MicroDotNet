package com.example.scanner.domain.usecase

import com.example.scanner.domain.models.GarbageInfo
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.domain.repository.GarbageRepository

class AddGarbageInfoUseCase(val garbageRepository: GarbageRepository) {
    fun execute(garbageInfo: GarbageInfo): RequestResult<String> {
        return garbageRepository.addGarbageInfo(garbageInfo)
    }
}