package com.example.scanner.domain.usecase

import android.widget.ImageView
import com.example.scanner.domain.repository.ImageRepository

class LoadImageUseCase(val imageRepository: ImageRepository) {
    fun execute(url: String, placeholder: Int, imageView: ImageView){
        imageRepository.loadPhoto(url, placeholder, imageView)
    }
}