package com.example.scanner.domain.models

class RequestResult<T>(
    val success: Boolean,
    val messages: List<String>,
    val data: List<T>?,
    val dataCount: Int,
    val responseCode: Int,) {
}

class RequestResultSingle<T>(
    val success: Boolean,
    val messages: List<String>,
    val data: T?,
    val responseCode: Int,) {
}