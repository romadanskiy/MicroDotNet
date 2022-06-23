package com.example.scanner.domain.repository

import android.widget.ImageView

interface ImageRepository {
    fun loadPhoto(url: String, placeholder: Int, imageView: ImageView)
}