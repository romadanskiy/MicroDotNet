package com.example.scanner.presentation

import android.R
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ListView
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import com.example.scanner.ApiFailedRequestResult
import com.example.scanner.databinding.FragmentReceptionPointsBinding
import com.example.scanner.domain.models.GetReceptionPoint
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.presentation.viewmodels.ErrorViewModel
import com.example.scanner.presentation.viewmodels.ReceptionPointsViewModel
import org.koin.androidx.viewmodel.ext.android.sharedViewModel

class ReceptionPointsFragment: Fragment() {

    private val receptionPointsViewModel by sharedViewModel<ReceptionPointsViewModel>()
    private val errorViewModel by sharedViewModel<ErrorViewModel>()
    private lateinit var binding: FragmentReceptionPointsBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?,
    ): View? {
        binding = FragmentReceptionPointsBinding.inflate(inflater, container, false);


        binding.goToAddPoints.setOnClickListener {
            var supportFragmentManager = activity?.supportFragmentManager;
            supportFragmentManager?.beginTransaction()
                ?.replace(com.example.scanner.R.id.fragment_container, AddReceptionPointsFragment())?.commit();
        }
        val loading =
            Observer<Boolean> { loading ->
                if (loading) {
                    binding.pointsList.visibility = View.GONE
                    binding.goToAddPoints.visibility = View.GONE
                    binding.progressBarPoints.visibility = View.VISIBLE
                } else {
                    binding.pointsList.visibility = View.VISIBLE
                    binding.goToAddPoints.visibility = View.VISIBLE
                    binding.progressBarPoints.visibility = View.GONE
                }
            }

        val pointsResult =
            Observer<RequestResult<GetReceptionPoint>> { result ->
                if (result != null && result.success) {
                    val adapter = ReceptionPointsAdapter(requireContext(), result.data!!)

                    binding.pointsList.adapter = adapter

                } else if (result != null) {
                    errorViewModel.handleRequestError(
                        ApiFailedRequestResult(
                            result.responseCode,
                            result.messages
                        ), ReceptionPointsFragment()
                    )
                    receptionPointsViewModel.clear()
                    val supportFragmentManager = activity?.supportFragmentManager
                    supportFragmentManager?.beginTransaction()
                        ?.replace(com.example.scanner.R.id.fragment_container, ErrorFragment())
                        ?.commit();
                }
            }


        receptionPointsViewModel.pointsResult.observe(viewLifecycleOwner, pointsResult)
        receptionPointsViewModel.loading.observe(viewLifecycleOwner, loading)
        receptionPointsViewModel.getPoints()
        return binding.root;
    }
}