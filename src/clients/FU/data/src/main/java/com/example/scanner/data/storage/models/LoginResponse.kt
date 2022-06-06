package com.example.scanner.data.storage.models

data class LoginResponse(
    var userId: Int,
    var accessToken: String,
    var refreshToken: String
)
