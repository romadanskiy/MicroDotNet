package com.example.scanner.presentation.login

import android.util.Log
import com.example.scanner.data.storage.models.BaseResponse
import com.example.scanner.data.storage.models.LoginResponse
import com.example.scanner.data.storage.retrofit2.RetrofitClient
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class LoginRepository {

    fun login(login: String, password: String, callback: (BaseResponse<LoginResponse>?) -> Unit) {
        val authService: AuthService = RetrofitClient.getClient().create(AuthService::class.java)

        var result: BaseResponse<LoginResponse>? = null
        authService.login(login, password).enqueue(object: Callback<BaseResponse<LoginResponse>> {

            override fun onResponse(
                call: Call<BaseResponse<LoginResponse>>,
                response: Response<BaseResponse<LoginResponse>>
            ) {
                result = response.body()
                result?.success = true
                Log.d("loggg", "1 $result")
                callback.invoke(result)
            }

            override fun onFailure(call: Call<BaseResponse<LoginResponse>>, t: Throwable) {
                result?.success = false
                Log.d("loggg", "2 $result")
                callback.invoke(result)
            }

        })

    }

}