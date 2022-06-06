package com.example.scanner.data.storage.models

abstract class BaseResponse<T> {
    var success: Boolean = false
    val messages: String? = null
    val data: T? = null
}