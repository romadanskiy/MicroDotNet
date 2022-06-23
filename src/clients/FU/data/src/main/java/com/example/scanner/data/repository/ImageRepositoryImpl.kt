package com.example.scanner.data.repository

import android.widget.ImageView
import com.example.scanner.domain.repository.ImageRepository
import com.squareup.picasso.Callback
import com.squareup.picasso.Picasso
import java.lang.Exception

class ImageRepositoryImpl: ImageRepository {
    override fun loadPhoto(url: String, placeholder: Int, imageView: ImageView) {
        val picasso = Picasso.get()
        picasso.isLoggingEnabled = true
        picasso
            .load(url)
            .noFade()
            .placeholder(placeholder)
            .centerCrop()
            .fit()
            .into(imageView)
    }
}
