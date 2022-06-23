package com.example.scanner.data.storage.models

import com.example.scanner.domain.models.GarbageCategory
import com.google.gson.annotations.SerializedName

class GetReceptionPoint(
    val id: Long,
    val name: String,
    val description: String?,
    val address: Address,
    val garbageTypes: List<GarbageCategory>
) {
}

class ReceptionPoint(
    val name: String,
    val description: String?,
    val address: String,
    val garbageTypes: List<Long>,
    val longitude: String = "longitude",
    val latitude: String = "latitude"
) {
}