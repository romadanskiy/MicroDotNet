package com.example.scanner.presentation.reg

import android.annotation.SuppressLint
import android.content.Context
import android.os.Bundle
import android.util.Log
import android.view.View
import androidx.fragment.app.Fragment
import com.example.scanner.Const.ACCESS_TOKEN
import com.example.scanner.Const.PREFS_NAME
import com.example.scanner.Const.REFRESH_TOKEN
import com.example.scanner.R
import com.example.scanner.data.storage.retrofit2.RetrofitClient
import com.example.scanner.presentation.MainActivity
import com.example.scanner.presentation.login.AuthService
import io.reactivex.Completable
import io.reactivex.Single
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.schedulers.Schedulers
import kotlinx.android.synthetic.main.fragment_reg.view.*

class RegFragment : Fragment(R.layout.fragment_reg) {

    @SuppressLint("CheckResult")
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {

        super.onViewCreated(view, savedInstanceState)
        view.reg_button.setOnClickListener {
            RetrofitClient.getClient().create(AuthService::class.java)
                .reg(
                    AuthService.RegRequest(
                        email = view.login.text.toString(),
                        password = view.password.text.toString(),
                        passwordRepeat = view.password2.text.toString()
                    )
                )
                .map {
                    requireContext()
                        .getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE)
                        .edit()
                        .apply {
                            putString(ACCESS_TOKEN, it.data?.accessToken)
                            putString(REFRESH_TOKEN, it.data?.refreshToken)
                        }
                        .apply()

                    it.success
                }
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe({
                    if (it)
                        (requireActivity() as MainActivity).toProfile()
                }, {})
        }

        view.to_login.setOnClickListener {
            (requireActivity() as MainActivity).toLogin()
        }
    }

}
