package com.example.scanner.presentation.add_point

import android.annotation.SuppressLint
import android.os.Bundle
import android.view.View
import android.widget.EditText
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.example.scanner.R
import com.example.scanner.data.storage.retrofit2.RetrofitClient
import com.example.scanner.presentation.login.AuthService
import io.reactivex.schedulers.Schedulers
import kotlinx.android.synthetic.main.fragment_add_point_info.*
import kotlinx.android.synthetic.main.fragment_add_point_info.view.*

class AddPointInfoFragment : Fragment(R.layout.fragment_add_point_info) {

    @SuppressLint("CheckResult")
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        view.add.setOnClickListener {
            RetrofitClient.getClient().create(PointService::class.java)
                .addPoint(
                    PointBody(
                        name = view.name.fetchText(),
                        description = view.description.fetchText(),
                        address = view.address.fetchText(),
                        latitude = view.latitude.fetchText(),
                        longitude = view.longitude.fetchText()
                    )
                )
                .subscribeOn(Schedulers.io())
                .subscribe({
                    if (!it.success) {
                        Toast.makeText(requireContext(), "Ошибка добавления", Toast.LENGTH_SHORT)
                            .show()
                    }
                }, { })
        }
    }

    private fun EditText.fetchText(): String = this.text.toString()

}
