package com.example.scanner

class ApiRequestResult<T>(code: Int, val success: Boolean, val data: List<T>?, errorMessages: List<String>): ApiFailedRequestResult(code, errorMessages) {

}

class ApiRequestResultSingle<T>(code: Int, val success: Boolean, val data: T?, errorMessages: List<String>): ApiFailedRequestResult(code, errorMessages) {

}

open class ApiFailedRequestResult(val code: Int, val errorMessages: List<String>)