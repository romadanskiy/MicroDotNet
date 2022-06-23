package com.example.scanner.domain.repository

import com.example.scanner.domain.models.*

interface ReceptionPointRepository {
    fun getReceptionPoints(categoryIds: List<Long>?) : RequestResult<GetReceptionPoint>

    fun addReceptionPoint(receptionPointInfo: ReceptionPoint) : RequestResult<String>
}