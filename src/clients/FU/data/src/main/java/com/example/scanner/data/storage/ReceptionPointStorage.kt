package com.example.scanner.data.storage

import com.example.scanner.data.storage.models.*

interface ReceptionPointStorage {
    fun getReceptionPoints(categoryIds: List<Long>?): ApiResult<GetReceptionPoint>

    fun addReceptionPoint(receptionPoint: ReceptionPoint): ApiResult<String>
}