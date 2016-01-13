using CustomControls;
using PurposeColor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class CommentsView : ContentView
    {
        CustomLayout masterLayout;
        CustomLayout pageContainedLayout;
        Label listTitle;
        int topSpacing = 75; // space between comments list and screen top end
        TapGestureRecognizer addCommentButtonTap;
        Image addCommentButton;
        PurposeColor.CustomControls.CustomEditor newCommentEntry;
        bool isPosting = false;
        PurposeColor.interfaces.IProgressBar progressBar = null;
        GemType curentGemType;
        string currentGemId;
        bool isCommunityShared = false;
        List<Comment> Comments;
        int heightSpacing = 95;
		StackLayout listContainer = null;
		TapGestureRecognizer removeLabelTap = null;
		StackLayout commentLayout = null;
		string currentUserId = "2"; ///  = 2 for testing
		User currentUser;

        public CommentsView(CustomLayout parentLayout, List<Comment> allComments, string gemId, GemType gemType, bool isCommunityGem)
        {

            /// for testing only /
            //allComments = null;

            pageContainedLayout = parentLayout;
            currentGemId = gemId;
            curentGemType = gemType;

            isCommunityShared = isCommunityGem;

            if (allComments != null)
            {
                if (allComments.Count < 4) // for popup height adjustment
                {
                    heightSpacing = 75;
                    topSpacing = 50;
                }

                Comments = allComments;
            }
            else
            {
                topSpacing = 30;        // for popup height adjustment
                heightSpacing = 50;
            }

			try {
				currentUserId = App.Settings.GetUser().UserId.ToString();
				currentUser = App.Settings.GetUser();
				if (currentUser == null) {
					currentUser = new User{ UserName = "me", UserId = 2, AllowCommunitySharing = true};
				}

			} catch (Exception ex) 
			{
			}
            masterLayout = new CustomLayout();
			progressBar = DependencyService.Get<PurposeColor.interfaces.IProgressBar>();

            #region EMPTY AREA TAP GESTURERECOGNIZER

            StackLayout layout = new StackLayout();
            layout.BackgroundColor = Color.Black;
            layout.Opacity = .5;
            layout.WidthRequest = App.screenWidth;
            layout.HeightRequest = App.screenHeight;

            masterLayout.AddChildToLayout(layout, 0, 0);

            TapGestureRecognizer emptyAreaTapGestureRecognizer = new TapGestureRecognizer();
            emptyAreaTapGestureRecognizer.Tapped += (s, e) =>
            {
                try
                {
                    View pickView = pageContainedLayout.Children.FirstOrDefault(pick => pick.ClassId == Constants.COMMENTS_VIEW_CLASS_ID);
                    pageContainedLayout.Children.Remove(pickView);
                    pickView = null;
                }
                catch (Exception)
                {
                }
            };
            layout.GestureRecognizers.Add(emptyAreaTapGestureRecognizer);

            #endregion

            #region LIST TITLE

            StackLayout listHeader = new StackLayout();
            listHeader.WidthRequest = App.screenWidth * 96 / 100;
            listHeader.HeightRequest = App.screenHeight * Device.OnPlatform(6, 6, 7) / 100;
            listHeader.BackgroundColor = Color.FromRgb(30, 126, 210);
            masterLayout.AddChildToLayout(listHeader, 2, (heightSpacing - topSpacing - 7));

            listTitle = new Label();
            listTitle.Text = "Comments";
            listTitle.TextColor = Color.White;
            listTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            listTitle.FontSize = Device.OnPlatform(15, 15, 20);
            masterLayout.AddChildToLayout(listTitle, Device.OnPlatform(3, 3, 3), (heightSpacing - topSpacing - Device.OnPlatform(6, 6, 5)));

            int fontSize = 15;
            if (App.screenDensity > 1.5)
            {
                fontSize = Device.OnPlatform(15, 17, 17);
            }
            else
            {
                fontSize = 15;
            }

            listTitle.FontSize = Device.OnPlatform(fontSize, fontSize, 20);

            #endregion

            listContainer = new StackLayout();
            listContainer.BackgroundColor = Color.White;
            listContainer.WidthRequest = App.screenWidth * 96 / 100;
			listContainer.Orientation = StackOrientation.Vertical;
            
			#region COMMENTS VIEW GENERATING


			if (Comments == null || Comments.Count < 1) 
            {
				listContainer.Children.Add (new StackLayout {
					Padding = 10,
					HeightRequest = 30,
					BackgroundColor = Color.White,
					WidthRequest = 100,
                    ClassId = "NoCommentContainer",
					Children = { new Label { Text = "Add your first comment..", 
                                            TextColor = Color.Gray,
                                            FontFamily = Constants.HELVERTICA_NEUE_LT_STD
                                            }
                    }
				});
                
			} else {
				foreach (var comment in Comments) {
					GenerateCommentView(comment);

				}// for each
			}
			#endregion

            #region INPUT
            newCommentEntry = new PurposeColor.CustomControls.CustomEditor
            {
                Placeholder = "Add new comment",
                HeightRequest = Device.OnPlatform(50, 50, 72),
                BackgroundColor = Color.Transparent,//Color.White,
                WidthRequest = App.screenWidth * .80,
                HorizontalOptions = LayoutOptions.Start,
                Text = Device.OnPlatform(string.Empty, string.Empty,"Add new Comment..")
                
            };

            addCommentButton = new Image();
            addCommentButton.Source = Device.OnPlatform("icon_send.png", "icon_send.png", "//Assets//icon_send.png");
            
            addCommentButton.VerticalOptions = LayoutOptions.Center;
            addCommentButton.HorizontalOptions = LayoutOptions.Center;
            addCommentButtonTap = new TapGestureRecognizer();
            addCommentButtonTap.Tapped += addCommentButtonTapped;
            addCommentButton.GestureRecognizers.Add(addCommentButtonTap);

            StackLayout inputCountainer = new StackLayout
            {
                Spacing = Device.OnPlatform(5, 5, 1),
                Padding = Device.OnPlatform(5, 5, 5),
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.White,
                Children = { newCommentEntry, addCommentButton }
            };


            #endregion

            #region OUTER CONTAINERS

            ScrollView scrollView = new ScrollView
            {
                Content = listContainer,
                //BackgroundColor = Color.White,
                HeightRequest = (App.screenHeight * topSpacing / 100) - Device.OnPlatform(90, 90, 180),
                IsClippedToBounds = true
            };

            StackLayout commentsAndInputs = new StackLayout
            {
                Spacing = 1,
                //Children = { new StackLayout { BackgroundColor = Color.White, Children = { scrollView, inputCountainer } }, new StackLayout { WidthRequest = App.screenWidth * 96, HeightRequest = 30, BackgroundColor = Color.Transparent } },
                Children = { scrollView, inputCountainer },
				BackgroundColor = Color.White,
                WidthRequest = App.screenWidth * .96, // should be same width as popup title bar.
                Orientation = StackOrientation.Vertical
            };

            masterLayout.AddChildToLayout(commentsAndInputs, 2, heightSpacing - topSpacing - Device.OnPlatform(1, 1, 1));
            
            #endregion

            if (Device.OS == TargetPlatform.WinPhone)
            {
                addCommentButton.WidthRequest = 60;
                addCommentButton.HeightRequest = 60;
            }

            Content = masterLayout;
        }

        async void RemoveLabelTap_Tapped (object sender, EventArgs e)
        {
			// sender Class id will be the comment id.

			try {

				progressBar.ShowProgressbar ("Removing");
				Label senderControl = sender as Label;
				senderControl.GestureRecognizers.Remove(removeLabelTap);
				string commentId = senderControl.ClassId;
				string responceCode = await PurposeColor.Service.ServiceHelper.RemoveComment(commentId);
				if (responceCode == "200") {
					// remove the corresponding comment from view
					var commentStackForRemoval = listContainer.Children.FirstOrDefault(s => s.ClassId == commentId);
					if (commentStackForRemoval != null) {
						listContainer.Children.Remove(commentStackForRemoval);
						commentStackForRemoval = null;
					}
				}else{
					progressBar.ShowToast("Network error, please try again later.");
				}
			} catch (Exception ex) {
				progressBar.HideProgressbar ();
			}

			progressBar.HideProgressbar ();
        }

        private async void addCommentButtonTapped(object sender, EventArgs e)
        {
            if (isPosting == true || string.IsNullOrEmpty(newCommentEntry.Text))
            {
                return;
            }

            string commentText = string.Empty;
            try
            {
                addCommentButtonTap.Tapped -= addCommentButtonTapped;
                
                isPosting = true;
                commentText = newCommentEntry.Text.Trim();

                progressBar.ShowProgressbar("Saving comment");
                isPosting = true;

                PurposeColor.Model.User user = App.Settings.GetUser();

                ////////////// for testing  //test //////////////

                isCommunityShared = false; /// for testing only 
                user = new PurposeColor.Model.User
                {
                    UserId = 2,
					AllowCommunitySharing = true,
					UserName = "Test user"
                };
				await App.Settings.SaveUser(user);


                ////////////// for testing  //test //////////////

                if (user == null)
                {
                    isPosting = false;
                    return;
                }
                string actionId = string.Empty;
                string eventId = string.Empty;
                string goalId = string.Empty;
                switch (curentGemType)
                {
                    case GemType.Goal:
                        goalId = currentGemId;
                        break;
                    case GemType.Event:
                        eventId = currentGemId;
                        break;
                    case GemType.Action:
                        actionId = currentGemId;
                        break;
                    case GemType.Emotion:
                        break;
                    default:
                        break;
                }
                string commentSharingCode = isCommunityShared ? "1" : "0"; /// "0" denotes its private comment
				string statusCode = await PurposeColor.Service.ServiceHelper.AddComment(user.UserId.ToString(), commentText, commentSharingCode, goalId, eventId, actionId);
                if (statusCode == "200")
                {
                    isPosting = false;
                    progressBar.HideProgressbar();
                    progressBar.ShowToast("Comment saved");

                    // add the new comment to the comments list view.
                }
                else
                {
                    progressBar.HideProgressbar();
                    progressBar.ShowToast("Could not save the comment now, please try again later");
                }
                

				try {
					
					//add comment to view
					Comment newComment = new Comment
					{
						comment_id = "000",
						comment_txt = commentText,
						user_id = currentUserId,
						firstname = user.UserName,
						profileurl = string.IsNullOrEmpty(user.ProfileImageUrl)?"admin/uploads/default/noprofile.png":user.ProfileImageUrl,// profile picture should be of this person// can take it from local. ==>  user.prfilePicUrl
						comment_datetime = DateTime.Now.ToLocalTime().ToString("f")
					};

                    //firstComment
                    var noCommentsLabel = listContainer.Children.FirstOrDefault(s => s.ClassId == "NoCommentContainer");
                    if (noCommentsLabel != null)
                    {
                        listContainer.Children.Remove(noCommentsLabel);
                        noCommentsLabel = null;
                    }

					GenerateCommentView(newComment);
                    
				} catch (Exception ex) {
				}

				newCommentEntry.Text = string.Empty;
            }
            catch (Exception)
            {
                isPosting = false;
                progressBar.ShowToast("Could not save the comment now, please try again later");
            }
            addCommentButtonTap.Tapped += addCommentButtonTapped;
        }

		void GenerateCommentView(Comment comment)
		{
			try {
				StackLayout TextHolderStack = new StackLayout {		// contain Name label , Comment label and date stack
					Orientation = StackOrientation.Vertical,
					Spacing = 0,
					HorizontalOptions = LayoutOptions.End,
                    Padding = new Thickness(0,10,0,0)
				};

				if (!string.IsNullOrEmpty (comment.comment_txt)) {
					commentLayout = new StackLayout {
                        Spacing = 0,
						Orientation = StackOrientation.Horizontal,
						ClassId = comment.comment_id,
						HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        Padding = 0
					};

					///////////////////////// profileIcone /////////////////////////
					Image profileIcone = new Image {
						HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
						HeightRequest = App.screenHeight * .15,
						WidthRequest = App.screenWidth * .15
					};

					if (!string.IsNullOrEmpty (comment.profileurl)) {

						profileIcone.Source = Constants.SERVICE_BASE_URL + comment.profileurl;
					} else {
						profileIcone.Source = Device.OnPlatform ("noimage.png", "noimage.png", "//Assets//noimage.png");
					}
                    

                    commentLayout.Children.Add(new StackLayout
                    {
                        Padding = new Thickness(5, Device.OnPlatform(-10, -10, -18), 5, 0),
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        HeightRequest = App.screenHeight * .12,
						WidthRequest = App.screenWidth * .12,
                        Children = { profileIcone }
                    });
                    //profileIcone.TranslationY = 0; // not working
					///////////////////////// commentText /////////////////////////
					Label commentTextLabel = new Label {
						Text = comment.comment_txt,
						TextColor = Color.Gray,
                        FontSize = Device.OnPlatform(14, 14, 18),
						WidthRequest = App.screenWidth * .70,
						HorizontalOptions = LayoutOptions.Start
					};

					///////////////////////// firstname /////////////////////////
					if (!string.IsNullOrEmpty (comment.firstname)) {
						Label nameText = new Label {
							Text = comment.firstname,
							TextColor = Color.Black,
							FontSize = Device.OnPlatform(16 ,16 ,21),
							HorizontalOptions = LayoutOptions.Start
						};
						TextHolderStack.Children.Add (nameText);
						TextHolderStack.Children.Add (commentTextLabel);
					}


					/////////////////////////  comment_datetime  /////////////////////////
					if (!string.IsNullOrEmpty (comment.comment_datetime)) {
						Label dateText = new Label {
							Text = comment.comment_datetime,
							TextColor = Color.Gray,
							FontSize = Device.OnPlatform(8, 8, 12),
							HorizontalOptions = LayoutOptions.Start
						};
						if (currentUserId == comment.user_id) {
							Label removeLabel = new Label {
								Text = "remove",
								TextColor = Color.FromRgb (30, 126, 210),
                                FontSize = Device.OnPlatform(8, 8, 12),
								HorizontalOptions = LayoutOptions.Start,
								ClassId = comment.comment_id
							};
							removeLabelTap = new TapGestureRecognizer ();
							removeLabelTap.Tapped += RemoveLabelTap_Tapped;
							removeLabel.GestureRecognizers.Add (removeLabelTap);
							TextHolderStack.Children.Add (new StackLayout {
								Children = { dateText, removeLabel },
								Orientation = StackOrientation.Horizontal,
								Spacing = 5,
                                Padding = 0,
								HorizontalOptions = LayoutOptions.Start
							});
						} else {
							TextHolderStack.Children.Add (dateText);
						}
					}
					commentLayout.Children.Add (TextHolderStack);

				}
				listContainer.Children.Add (commentLayout);
                

			} catch (Exception) {
			}
		}

    }
}
