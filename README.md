Adding something to state:

////
@GetMapping("/{userId}")
	public ResponseEntity<?> searchUserPlaces(@PathVariable long userId, OAuth2Authentication auth) throws NotFoundUserException {
		log.info("GET /places/searches/{}", userId);
		
		List<LightPlaceDTO> places = placeService.searchByUserId(userId);
		
		HttpStatus httpStatus = HttpStatus.OK;
		
//		System.out.println(auth.getOAuth2Request().getClientId());
//		System.out.println(auth.getName()); // Give me userId !
		
		if(places == null || places.isEmpty())
			httpStatus = HttpStatus.NOT_FOUND;
		
		return new ResponseEntity<List<LightPlaceDTO>>(places, httpStatus);
	}