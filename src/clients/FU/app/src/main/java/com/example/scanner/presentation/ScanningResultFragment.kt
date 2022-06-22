package com.example.scanner.presentation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.GridLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.scanner.ApiRequestResult
import com.example.scanner.ApiRequestResultSingle
import com.example.scanner.R
import com.example.scanner.databinding.FragmentScanningResultBinding
import com.example.scanner.domain.models.GarbageInfo
import com.example.scanner.domain.models.GetGarbageInfo
import com.example.scanner.presentation.viewmodels.ErrorViewModel
import com.example.scanner.presentation.viewmodels.ScannerViewModel
import com.example.scanner.presentation.viewmodels.ScanningResultViewModel
import org.koin.androidx.viewmodel.ext.android.sharedViewModel


class ScanningResultFragment : Fragment() {

    private val scannerViewModel by sharedViewModel<ScannerViewModel>()
    private val scanningResultViewModel by sharedViewModel<ScanningResultViewModel>()
    private val errorViewModel by sharedViewModel<ErrorViewModel>()
    private lateinit var binding: FragmentScanningResultBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentScanningResultBinding.inflate(inflater, container, false);

        val recyclerView: RecyclerView = binding.garbageTypesRecyclerView
        recyclerView.layoutManager = GridLayoutManager(this.context, 3)

        binding.addGarbage.setOnClickListener {
            scanningResultViewModel.clear()
            var supportFragmentManager = activity?.supportFragmentManager;
            supportFragmentManager?.beginTransaction()
                ?.replace(R.id.fragment_container, AddGarbageFragment())?.commit();
        }

        val scanningResult =
            Observer<ApiRequestResultSingle<GetGarbageInfo>> { result ->
                if (result != null && result.success) {
                    val garbageInfo = result.data!!
                    binding.name.text = garbageInfo.name
                    binding.description.text = garbageInfo.description
                    showSuccess()
                    if (garbageInfo.garbageCategories != null) {
                        recyclerView.adapter =
                            GarbageCategoriesRecyclerAdapter(garbageInfo.garbageCategories!!)
                    }
                    scanningResultViewModel.loadImage(R.drawable.garbage_not_found, binding.garbageImage)
                } else if (result != null) {
                    showError()
                }
            }

        val loading =
            Observer<Boolean> { loading ->
                if (loading) {
                    hideElements()
                    binding.progressBar.visibility = View.VISIBLE
                } else {
                    binding.progressBar.visibility = View.GONE
                }
            }

        scanningResultViewModel.garbageInfo.observe(viewLifecycleOwner, scanningResult)
        scanningResultViewModel.loading.observe(viewLifecycleOwner, loading)
        scanningResultViewModel.getGarbageInfoByBarcode(scannerViewModel.barcode.value!!)

        return binding.root;
    }

    private fun hideElements() {
        binding.garbageImage.visibility = View.GONE
        binding.nameHeader.visibility = View.GONE
        binding.name.visibility = View.GONE
        binding.description.visibility = View.GONE
        binding.descriptionHeader.visibility = View.GONE
        binding.categoryHeader.visibility = View.GONE
        binding.garbageTypesRecyclerView.visibility = View.INVISIBLE
        binding.imageNotFound.visibility = View.GONE
        binding.notFoundHeader.visibility = View.GONE
        binding.notFoundText.visibility = View.GONE
        binding.addGarbage.visibility = View.GONE
        binding.goToUtilization.visibility = View.GONE
    }

    private fun showSuccess() {
        binding.garbageImage.visibility = View.VISIBLE
        binding.nameHeader.visibility = View.VISIBLE
        binding.name.visibility = View.VISIBLE
        binding.descriptionHeader.visibility = View.VISIBLE
        binding.description.visibility = View.VISIBLE
        binding.categoryHeader.visibility = View.VISIBLE
        binding.garbageTypesRecyclerView.visibility = View.VISIBLE
        binding.goToUtilization.visibility = View.VISIBLE
        binding.progressBar.visibility = View.GONE

    }

    private fun showError() {
        binding.imageNotFound.visibility = View.VISIBLE
        binding.notFoundHeader.visibility = View.VISIBLE
        binding.notFoundText.visibility = View.VISIBLE
        binding.addGarbage.visibility = View.VISIBLE
        binding.notFoundText.text =
            "Сейчас в системе нет информации о товаре со штрихкодом " + scannerViewModel.barcode.value!!.barcode
    }
}