//
//  ProfilePageViewController.swift
//  Buddy
//
//  Created by Morgan Wilkinson on 7/7/19.
//  Copyright © 2019 Morgan Wilkinson. All rights reserved.
//

import UIKit

class ProfilePageViewController: UIPageViewController {
    // load view required
    override func viewDidLoad() {
        super.viewDidLoad()
        
        dataSource = self
        
        if let firstViewController = viewControllersArray.first{
            setViewControllers([firstViewController], direction: UIPageViewController.NavigationDirection.forward, animated: true, completion: nil)
            }
    }
}
    
extension ProfilePageViewController: UIPageViewControllerDataSource{
    
    //View Controller Before
    func pageViewController(_ pageViewController: UIPageViewController, viewControllerBefore viewController: UIViewController) -> UIViewController?
    {
        guard let viewControllerIndex = viewControllersArray.firstIndex(of: viewController) else {
            return nil
        }
        
        let previousIndex = viewControllerIndex - 1
        
        // if the statement is true ignore this else if the statement is false retun nil as previousIndex is less than 0.
        guard previousIndex >= 0 else {
            return nil
        }
        
        // if the array count is bigger than the previous index continue on as there are more views to see.
        guard viewControllersArray.count > previousIndex else{
            return nil
        }
        
        return viewControllersArray[previousIndex]
    }
    
    //View Controller After
    func pageViewController(_ pageViewController: UIPageViewController, viewControllerAfter viewController: UIViewController) -> UIViewController?
    {
        guard let viewControllerIndex = viewControllersArray.firstIndex(of: viewController) else {
            return nil
        }
        
        let nextIndex = viewControllerIndex + 1
        let viewControllersArrayCount = viewControllersArray.count
        
        guard viewControllersArrayCount != nextIndex else{
            return nil
        }
        
        guard viewControllersArrayCount > nextIndex else{
            return nil
        }
        
        return viewControllersArray[nextIndex]
    }
    
    func presentationCount(for pageViewController: UIPageViewController) -> Int
    {
        return viewControllersArray.count
    }
    
    func presentationIndex(for pageViewController: UIPageViewController) -> Int
    {
        guard let firstViewController = viewControllers?.first, let firstViewControllerIndex = viewControllersArray.firstIndex(of: firstViewController) else{
            return 0
        }
        
        return firstViewControllerIndex
    }
    
    
}

private var viewControllersArray: [UIViewController] = {
    return [newViewController(name: "Overview"), newViewController(name: "Detail")]
}()

// Get viewcontroller from Storyboard
private func newViewController(name: String) -> UIViewController
{
    return UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "\(name)") as UIViewController
}
