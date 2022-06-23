package com.example.scanner.domain.usecase

import com.example.scanner.domain.models.GetReceptionPoint
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.domain.models.RequestResultSingle
import com.example.scanner.domain.repository.ReceptionPointRepository

class GetReceptionPointsUseCase(private val receptionPointRepository: ReceptionPointRepository) {
    fun execute(categoryIds: List<Long>?): RequestResult<GetReceptionPoint> {
        return receptionPointRepository.getReceptionPoints(categoryIds)
    }
}