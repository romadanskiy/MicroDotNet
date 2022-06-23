package com.example.scanner.presentation

import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import com.example.scanner.ApiFailedRequestResult
import com.example.scanner.ApiRequestResult
import com.example.scanner.R
import com.example.scanner.databinding.FragmentAddGarbageBinding
import com.example.scanner.databinding.FragmentAddReceptionPointBinding
import com.example.scanner.domain.models.GarbageCategory
import com.example.scanner.domain.models.GarbageInfo
import com.example.scanner.domain.models.ReceptionPoint
import com.example.scanner.presentation.viewmodels.*
import org.koin.androidx.viewmodel.ext.android.sharedViewModel

class AddReceptionPointsFragment: Fragment() {

    private val addReceptionPointViewModel by sharedViewModel<AddReceptionPointViewModel>()
    private val errorViewModel by sharedViewModel<ErrorViewModel>()
    private val scannerViewModel by sharedViewModel<ScannerViewModel>()
    private val garbageCategoriesViewModel by sharedViewModel<GarbageCategoriesViewModel>()
    private lateinit var binding: FragmentAddReceptionPointBinding

    private val selectImageCode = 100000001
    private val requestCodeExternalStoragePermission = 1002

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentAddReceptionPointBinding.inflate(inflater, container, false);

        binding.savePoint.setOnClickListener {
            saveDataToViewModel()
            val info = addReceptionPointViewModel.pontInfo.value
            if (info != null)
                addReceptionPointViewModel.addPointInfo()
        }

        binding.setGarbageCategoriesToPoint.setOnClickListener {
            saveDataToViewModel()
            var supportFragmentManager = activity?.supportFragmentManager;
            supportFragmentManager?.beginTransaction()
                ?.replace(R.id.fragment_container, GarbageCategoriesFragment(true))?.commit();
        }

        val info =
            Observer<ReceptionPoint> { info ->

                if (info != null) {
                    binding.pointNameEdit.setText(info.name)
                    binding.pointDescriptionEdit.setText(info.description)
                    binding.pointAddressEdit.setText(info.address)
                }
            }

        val result =
            Observer<ApiRequestResult<String>> { result ->
                if (result != null) {
                    if (result.success) {
                        var supportFragmentManager = activity?.supportFragmentManager;
                        supportFragmentManager?.beginTransaction()
                            ?.replace(R.id.fragment_container, ReceptionPointsFragment())?.commit();
                        addReceptionPointViewModel.clear()
                    } else {

                        errorViewModel.handleRequestError(
                            ApiFailedRequestResult(
                                result.code,
                                result.errorMessages
                            ), AddReceptionPointsFragment()
                        )
                        addReceptionPointViewModel.setResult(null)
                        addReceptionPointViewModel.setLoading(false)
                        val supportFragmentManager = activity?.supportFragmentManager
                        supportFragmentManager?.beginTransaction()
                            ?.replace(com.example.scanner.R.id.fragment_container, ErrorFragment())
                            ?.commit();
                    }
                }
            }

        val loading =
            Observer<Boolean> { loading ->
                if (loading) {
                    binding.addPointLoader.visibility = View.VISIBLE
                    showFields(false)
                } else {
                    binding.addPointLoader.visibility = View.GONE
                    showFields(true)
                }
            }

        addReceptionPointViewModel.result.observe(viewLifecycleOwner, result)
        addReceptionPointViewModel.loading.observe(viewLifecycleOwner, loading)
        addReceptionPointViewModel.pontInfo.observe(viewLifecycleOwner, info)
        addReceptionPointViewModel.setLoading(false)

        return binding.root;
    }

    private fun saveDataToViewModel() {
        var categories = emptyList<Long>()
        if (garbageCategoriesViewModel.categoryIds.value != null) {
            categories = garbageCategoriesViewModel.categoryIds.value!!
        }

        val pointInfo = ReceptionPoint(
            binding.pointNameEdit.text.toString(),
            binding.pointDescriptionEdit.text.toString(),
            binding.pointAddressEdit.text.toString(),
            categories,
        )
        addReceptionPointViewModel.setPointInfo(pointInfo)
    }

    private fun showFields(show: Boolean) {
        var visibility = View.GONE
        if (show) {
            visibility = View.VISIBLE
        }
        binding.setGarbageCategoriesToPoint.visibility = visibility
        binding.pointNameEdit.visibility = visibility
        binding.pointNameText.visibility = visibility
        binding.pointDescriptionText.visibility = visibility
        binding.pointDescriptionEdit.visibility = visibility
        binding.pointAddressEdit.visibility = visibility
        binding.pointAddressText.visibility = visibility
        binding.savePoint.visibility = visibility
    }
}