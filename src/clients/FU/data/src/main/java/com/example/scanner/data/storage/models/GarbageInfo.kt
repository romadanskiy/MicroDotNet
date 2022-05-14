package com.example.scanner.data.storage.models


class GarbageInfo(
    var name: String,
    var description: String,
    var garbageCategories: List<GarbageCategory>,
    var success: Boolean
) {
}