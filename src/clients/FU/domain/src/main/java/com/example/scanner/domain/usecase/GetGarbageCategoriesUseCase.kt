package com.example.scanner.domain.usecase

import com.example.scanner.domain.models.GarbageCategory
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.domain.repository.GarbageRepository

class GetGarbageCategoriesUseCase(val garbageRepository: GarbageRepository) {
    fun execute(): RequestResult<GarbageCategory> {
        return garbageRepository.getGarbageCategories()
    }
}