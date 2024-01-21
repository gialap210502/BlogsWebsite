import { Component, OnInit } from '@angular/core';
import { BlogPostService } from '../../blog-post/services/blog-post.service';
import { Observable } from 'rxjs';
import { BlogPost } from '../../blog-post/models/blog-post.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{

  blog$?: Observable<BlogPost[]>;

  constructor(private blogPostService: BlogPostService) {
    
  }
  ngOnInit(): void {
    this.blog$ = this.blogPostService.getAllBlogPosts();
  }


}
