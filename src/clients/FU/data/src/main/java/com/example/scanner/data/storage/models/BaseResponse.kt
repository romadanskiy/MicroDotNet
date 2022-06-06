package com.example.scanner.data.storage.models

open class BaseResponse<T> {
    var success: Boolean = false
    val messages: List<String>? = null
    val data: T? = null
}