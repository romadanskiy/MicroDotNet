package com.example.scanner.presentation.login

import android.annotation.SuppressLint
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.example.scanner.R
import com.example.scanner.data.storage.retrofit2.RetrofitClient
import com.example.scanner.presentation.MainActivity
import com.example.scanner.presentation.UtilizationFragment
import com.example.scanner.presentation.reg.RegFragment
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.schedulers.Schedulers
import kotlinx.android.synthetic.main.fragment_login.view.*

class LoginFragment : Fragment(R.layout.fragment_login) {

    @SuppressLint("CheckResult")
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        view.login_button.setOnClickListener {
            RetrofitClient.getClient().create(AuthService::class.java)
                .login(AuthService.LoginRequest(
                    email = view.login.text.toString(),
                    password = view.password.text.toString())
                )
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe({
                    when (it.success) {
                        false -> Toast.makeText(
                            requireContext(),
                            it.messages?.joinToString(" "),
                            Toast.LENGTH_SHORT
                        ).show()
                    }
                }, {})
        }

        view.register.setOnClickListener {
            (requireActivity() as MainActivity).toReq()
        }
    }

}
