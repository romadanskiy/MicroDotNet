package com.example.scanner.presentation.login

import com.example.scanner.data.storage.models.BaseResponse
import com.example.scanner.data.storage.models.LoginResponse
import retrofit2.Call
import retrofit2.http.POST
import retrofit2.http.Query

interface AuthService {

    @POST("Account/Login")
    fun login(
        @Query("email") email: String,
        @Query("password") password: String
    ): Call<BaseResponse<LoginResponse>>

}