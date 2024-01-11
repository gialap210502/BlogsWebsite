import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { BlogPost } from '../models/blog-post.model';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../category/models/category.model';

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrls: ['./edit-blogpost.component.css']
})
export class EditBlogpostComponent implements OnInit, OnDestroy{

  id: string | null = null;
  model?: BlogPost;
  categories$?: Observable<Category[]>;
  selectedcatgories?: string[]

  routeSubscription?: Subscription;

  constructor(private route: ActivatedRoute,
    private blogPostService: BlogPostService,
    private categoryService: CategoryService) {

    
  }


  ngOnInit(): void {

    this.categories$ = this.categoryService.getAllCategories();

    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');


        // get Blogpost from api
        if(this.id){
          this.blogPostService.getBlogPostById(this.id).subscribe({
            next: (response) => {
              this.model = response;
              this.selectedcatgories = response.categories.map(x => x.id);
            }
          });
        }
        
      }
    })
  }

  onFormSubmit(): void {

  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
  }

}
