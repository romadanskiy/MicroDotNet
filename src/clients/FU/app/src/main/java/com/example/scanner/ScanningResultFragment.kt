package com.example.scanner

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import com.example.scanner.databinding.FragmentScanningResultBinding
import com.example.scanner.interfaces.GarbageApi
import com.example.scanner.models.ScanningResultViewModel
import com.example.scanner.models.dto.GarbageInfo
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory


class ScanningResultFragment: Fragment() {

    private val scanningResultViewModel: ScanningResultViewModel by activityViewModels()
    private lateinit var binding: FragmentScanningResultBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentScanningResultBinding.inflate(inflater, container, false);

        val BASE_URL = "http://192.168.1.67:5000/"

        var retrofit = Retrofit.Builder()
            .baseUrl(BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()

        val garbageApi: GarbageApi = retrofit.create(GarbageApi::class.java)
        garbageApi.
        getGarbageInfo(scanningResultViewModel.scanningResult.value!!)
        ?.enqueue(object: Callback<GarbageInfo> {
            override fun onResponse(call: Call<GarbageInfo>, response: Response<GarbageInfo>) {
                binding.garbageInfoViewModel = response.body();
                binding.lifecycleOwner = viewLifecycleOwner
                binding.name.text = response.body()?.name;
                binding.description.text = response.body()?.description;
                binding.garbageTypes.text = response.body()?.garbageTypes;
            }

            override fun onFailure(call: Call<GarbageInfo>, t: Throwable) {
                binding.name.text = t.message;
            }
        }
        );

        return binding.root;
    }


}