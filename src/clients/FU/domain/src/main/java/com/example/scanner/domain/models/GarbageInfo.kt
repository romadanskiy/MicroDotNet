package com.example.scanner.domain.models

import android.graphics.Bitmap
import java.io.File

class GarbageInfo(
    val name: String,
    val description: String?,
    val garbageCategories: List<GarbageCategory>?,
    val barcode: String,
    val image: File?
) {
}

class GetGarbageInfo(
    val name: String,
    val description: String?,
    val garbageCategories: List<GarbageCategory>?,
    val barcode: String,
    val image: String?
) {
}