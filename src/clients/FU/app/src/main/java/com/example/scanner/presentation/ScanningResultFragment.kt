package com.example.scanner.presentation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import com.example.scanner.databinding.FragmentScanningResultBinding
import com.example.scanner.models.ScanningResultViewModel
import com.example.scanner.models.dto.GarbageInfo
import com.example.scanner.presentation.viewmodels.ScannerViewModel
import org.koin.androidx.viewmodel.ext.android.sharedViewModel
import org.koin.androidx.viewmodel.ext.android.viewModel
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory


class ScanningResultFragment : Fragment() {

    private val scannerViewModel by sharedViewModel<ScannerViewModel>()
    private lateinit var binding: FragmentScanningResultBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentScanningResultBinding.inflate(inflater, container, false);

        var t = scannerViewModel.barcode;



        return binding.root;
    }


}