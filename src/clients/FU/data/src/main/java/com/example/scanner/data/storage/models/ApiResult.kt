package com.example.scanner.data.storage.models

class ApiResult<T> (
    val success: Boolean,
    val messages: List<String>,
    val data: List<T>?,
    var responseCode: Int
){

}

class ApiResultSingle<T> (
    val success: Boolean,
    val messages: List<String>,
    val data: T?,
    var responseCode: Int
){

}