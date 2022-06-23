package com.example.scanner.domain.usecase

import com.example.scanner.domain.models.ReceptionPoint
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.domain.repository.ReceptionPointRepository

class AddReceptionPointUseCase(private val receptionPointRepository: ReceptionPointRepository) {
    fun execute(receptionPoint: ReceptionPoint): RequestResult<String> {
        return receptionPointRepository.addReceptionPoint(receptionPoint)
    }
}