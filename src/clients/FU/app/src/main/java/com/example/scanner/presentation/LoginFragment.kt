package com.example.scanner.presentation

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.example.scanner.R
import com.example.scanner.presentation.login.LoginRepository
import kotlinx.android.synthetic.main.fragment_login.view.*
import kotlinx.coroutines.launch

class LoginFragment: Fragment(R.layout.fragment_login) {

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        view.login_button.setOnClickListener {
            lifecycleScope.launch {
                val a = LoginRepository().login("a@a.a1", "a1s2d3f41") {
                    Log.d("loggg", "aaa $it")
                }
            }
        }
    }

}
