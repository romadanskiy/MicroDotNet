package com.example.scanner.domain.models

class GetReceptionPoint(
    val id: Long,
    val name: String,
    val description: String?,
    val address: Address,
    val garbageTypes: List<GarbageCategory>
) {
}

class ReceptionPoint(
    val id: Long,
    val name: String,
    val description: String?,
    val address: String,
    val garbageTypes: List<Long>,
    val longitude: String = "longitude",
    val latitude: String = "latitude"
) {
}