package com.example.scanner.presentation.login

import com.example.scanner.data.storage.models.BaseResponse
import com.example.scanner.data.storage.models.LoginResponse
import io.reactivex.Single
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.POST
import retrofit2.http.Query

interface AuthService {

    @POST("Account/Login")
    fun login(
        @Body login: LoginRequest
    ): Single<BaseResponse<LoginResponse>>

    @POST("Account/Register")
    fun reg(
        @Body login: RegRequest
    ): Single<BaseResponse<RegResponse>>

    data class LoginRequest(val email: String, val password: String)
    data class RegRequest(val email: String, val password: String, val passwordRepeat: String)

}

data class RegResponse(
    val accessToken: String,
    val refreshToken: String
)
