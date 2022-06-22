package com.example.scanner.data.storage.models

import android.graphics.Bitmap
import com.google.gson.annotations.SerializedName
import java.io.File


class GarbageInfo(
    val name: String,
    val description: String? = null,
    @SerializedName("garbageTypes")
    val garbageCategories: List<GarbageCategory>,
    val barcode: String,
    val image: File?
) {
}

class GetGarbageInfo(
    val name: String,
    val description: String? = null,
    @SerializedName("garbageTypes")
    val garbageCategories: List<GarbageCategory>,
    val barcode: String,
    val imagePath: String?
) {
}